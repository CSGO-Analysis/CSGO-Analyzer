using DemoParser_Core;
using DemoParser_Core.Entities;
using DemoParser_Core.Events;
using DemoParser_Model;
using DemoParser_Model.Observers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Timers = System.Timers.Timer;

namespace DemoParser_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DemoParser demoParser;
		private ScoreboardObserver scoreboardObserver;
		private CheatObserver cheatObserver;

		private ManualResetEvent locker = new ManualResetEvent(true);
		private BackgroundWorker bw;
		private bool IsWorkerPaused = false;

		private Timers timer;
		private int lastTick;
		private int avgTick;
		private int nbSeconds;

		private Stopwatch t1;

        public MainWindow()
        {
            InitializeComponent();

			timer = new Timers();
			timer.AutoReset = true;
			timer.Interval = 1000;
			timer.Elapsed += timer_Elapsed;
        }

		private void InitUI()
		{
			this.textboxLoad.Text = string.Empty;
			this.textblockContent.Text = string.Empty;
			this.buttonAnalyze.IsEnabled = true;
			this.buttonPause.IsEnabled = false;
			this.progressBar.Value = 0;
			this.labelProgress.Content = string.Empty;

			this.lastTick = 0;
			this.avgTick = 0;
			this.nbSeconds = 0;

			this.tabItemData.IsEnabled = false;
			this.tabItemScoreboard.IsEnabled = false;
			this.tabControl.SelectedItem = this.tabItemConsole;
		}

		private void InitParser(Stream demoFile)
		{
			demoParser = new DemoParser(demoFile);
			demoParser.EventsManager.HeaderParsed += demoParser_HeaderParsed;
			demoParser.EventsManager.MatchStarted += demoParser_MatchStarted;
			demoParser.EventsManager.MatchEnded += demoParser_MatchEnded;
			demoParser.EventsManager.RoundStart += demoParser_RoundStart;
			demoParser.EventsManager.PlayerKilled += demoParser_PlayerKilled;
			demoParser.EventsManager.RoundEnd += demoParser_RoundEnd;
			demoParser.EventsManager.RoundMvp += demoParser_RoundMvp;
			//demoParser.EventsManager.PlayerChat += demoParser_PlayerChat;
			//demoParser.EventsManager.WeaponFired += demoParser_WeaponFired;
			demoParser.EventsManager.TeamParsed += demoParser_TeamParsed;

			scoreboardObserver = new ScoreboardObserver(this.demoParser);
			cheatObserver = new CheatObserver(this.demoParser);
		}

		void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			Dispatcher.Invoke(new Action(() =>
			{
				nbSeconds++;
				this.avgTick += (demoParser.CurrentTick - lastTick);
				this.labelSpeed.Content = (demoParser.CurrentTick - lastTick) + " ticks/s - avg " + avgTick / nbSeconds + " ticks/s";
				this.lastTick = demoParser.CurrentTick;
			}
			));
		}

		#region Buttons events
		private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
			OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".dem"; // Default file extension
            dialog.Filter = "CSGO demo file (.dem)|*.dem"; // Filter files by extension

            if (dialog.ShowDialog() == true)
            {
				InitUI();

				this.textboxLoad.Text = dialog.FileName;

				FileStream demoFile = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				InitParser(demoFile);
            }
        }

		private void buttonAnalyze_Click(object sender, RoutedEventArgs e)
		{
			this.buttonLoad.IsEnabled = false;
			this.buttonAnalyze.IsEnabled = false;
			this.buttonPause.IsEnabled = true;
			this.t1 = Stopwatch.StartNew();
			timer.Start();

			demoParser.ParseDemo(false);

			bw = new BackgroundWorker();
			bw.WorkerReportsProgress = true;
			bw.WorkerSupportsCancellation = true;

			// what to do in the background thread
			bw.DoWork += new DoWorkEventHandler(
			delegate(object o, DoWorkEventArgs args)
			{
				BackgroundWorker b = o as BackgroundWorker;

				while (demoParser.ParseNextTick() && !bw.CancellationPending)
				{
					if (demoParser.Header.PlaybackTicks == 0)
						continue;

					// report the progress in percent
					b.ReportProgress(Convert.ToInt32(((float)demoParser.CurrentTick / demoParser.Header.PlaybackTicks) * 100), demoParser);
					// ability to pause/resume thread
					locker.WaitOne(Timeout.Infinite);
				}

				if (bw.CancellationPending)
				{
					args.Cancel = true;
					return;
				}
			});

			// what to do when progress changed (update the progress bar for example)
			bw.ProgressChanged += new ProgressChangedEventHandler(
			delegate(object o, ProgressChangedEventArgs args)
			{
				this.progressBar.Value = args.ProgressPercentage;
				this.labelProgress.Content = args.ProgressPercentage + "% - Tick : " + ((DemoParser)args.UserState).CurrentTick + "/" + ((DemoParser)args.UserState).Header.PlaybackTicks;
			});

			// what to do when worker completes its task (notify the user)
			bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
			delegate(object o, RunWorkerCompletedEventArgs args)
			{
				bw.CancelAsync();
				bw.Dispose();
				t1.Stop();
				timer.Stop();
				this.textblockContent.Text += "Finished!" + Environment.NewLine;
				this.textblockContent.Text += "Time: " + t1.Elapsed.ToString(@"ss\.fffffff");
				this.dataGridTeams.ItemsSource = demoParser.Teams;
				this.dataGridPlayers.ItemsSource = demoParser.Players;
				this.dataGridScoreboard.ItemsSource = scoreboardObserver.scoreBoard.GetLines();
				SortScoreboard();
				this.dataGridCheatAnalyze.ItemsSource = cheatObserver.results;
				this.buttonPause.IsEnabled = false;
				this.tabItemData.IsEnabled = true;
				this.tabItemScoreboard.IsEnabled = true;
			});

			bw.Disposed += new EventHandler(
			delegate(object s, EventArgs ea)
			{
				this.buttonLoad.IsEnabled = true;
			});

			bw.RunWorkerAsync();
		}

		private void buttonPause_Click(object sender, RoutedEventArgs e)
		{
			if (!IsWorkerPaused)
			{
				timer.Stop();
				locker.Reset();
				IsWorkerPaused = true;
				this.buttonPause.Content = '4';
			}
			else
			{
				timer.Start();
				locker.Set();
				IsWorkerPaused = false;
				this.buttonPause.Content = ';';
			}
		}
		#endregion

		#region Events handlers
		void demoParser_HeaderParsed(object sender, HeaderParsedEventArgs e)
		{
			Dispatcher.Invoke(new Action(() => 
				{
					this.textblockContent.Text = demoParser.Header.ToString();
					this.scrollViewer.ScrollToBottom();
				}
			));
		}

		void demoParser_WeaponFired(object sender, WeaponFiredEventArgs e)
		{
			Dispatcher.Invoke(new Action(() =>
				{
					this.textblockContent.Text += String.Format("Weapon_fired: {0} {1} {2} {3}", e.Player.Name, e.Weapon.OriginalString, e.Player.ViewDirectionX, e.Player.ViewDirectionY) + Environment.NewLine;
					this.scrollViewer.ScrollToBottom();
				}
			));
		}

		void demoParser_PlayerChat(object sender, PlayerChatEventArgs e)
		{
			Dispatcher.Invoke(new Action(() =>
			{
				this.textblockContent.Text += String.Format("[{0}] {1} > {2}", (e.TeamOnly) ? "t_say" : "say", e.Player.Name, e.Message) + Environment.NewLine;
				this.scrollViewer.ScrollToBottom();
			}
			));
		}

		void demoParser_RoundStart(object sender, RoundStartedEventArgs e)
		{
			Dispatcher.Invoke(new Action(() =>
				{
					if (Double.IsNaN(demoParser.CurrentTime))
						return;

					this.textblockContent.Text += "Round started " + TimeSpan.FromSeconds(demoParser.CurrentTime).ToString(@"hh\:mm\:ss") + Environment.NewLine;
					this.scrollViewer.ScrollToBottom();
				}
			));
		}

		void demoParser_RoundEnd(object sender, RoundEndedEventArgs e)
		{
			Dispatcher.Invoke(new Action(() =>
			{
				if (Double.IsNaN(demoParser.CurrentTime))
					return;

				this.textblockContent.Text += "Round ended (" + e.Message + ") " + TimeSpan.FromSeconds(demoParser.CurrentTime).ToString(@"hh\:mm\:ss") + Environment.NewLine;
				this.scrollViewer.ScrollToBottom();
			}
			));
		}

		void demoParser_MatchStarted(object sender, MatchStartedEventArgs e)
		{
			Dispatcher.Invoke(new Action(() =>
				this.textblockContent.Text += "Match started" + Environment.NewLine
			));
		}

		void demoParser_MatchEnded(object sender, MatchEndedEventArgs e)
		{
			Dispatcher.Invoke(new Action(() =>
				{
					this.textblockContent.Text += "Match ended" + Environment.NewLine;
				}
			));
		}

		void demoParser_PlayerKilled(object sender, PlayerKilledEventArgs e)
		{
			Dispatcher.Invoke(new Action(() =>
				{
					this.textblockContent.Text += String.Format("{0}{4} killed {1} with {2} {3}", e.Killer.Name, e.Victim.Name, e.Weapon.OriginalString, (e.Headshot) ? "(headshot)" : "", (e.Assist != null) ? " + " + e.Assist.Name : "") + Environment.NewLine;
					this.scrollViewer.ScrollToBottom();
				}
			));
		}

		void demoParser_RoundMvp(object sender, RoundMvpEventArgs e)
		{
			Dispatcher.Invoke(new Action(() =>
				{
					this.textblockContent.Text += String.Format("Round MVP : {0}; reason: {1}", e.Player.Name, e.Reason) + Environment.NewLine;
					this.scrollViewer.ScrollToBottom();
				}
			));
		}

		void demoParser_TeamParsed(object sender, TeamParsedEventArgs e)
		{
			Dispatcher.Invoke(new Action(() =>
			{
				if (e.Team.Side != DemoParser_Core.Entities.Team.TeamSide.Spectator)
				{
					if (e.Team.Num == 2)
					{
						this.imageFlag1.Source = new BitmapImage(new Uri("Resources/Flags/" + e.Team.Flag + ".png", UriKind.Relative));
						this.labelTeam1.Content = e.Team.Name;
						this.labelScore1.Content = String.Format("{0} ({1}:{2})", e.Team.Score, e.Team.Side, e.Team.ScoreFirstHalf);
					}
					else if (e.Team.Num == 3)
					{
						this.imageFlag2.Source = new BitmapImage(new Uri("Resources/Flags/" + e.Team.Flag + ".png", UriKind.Relative));
						this.labelTeam2.Content = e.Team.Name;
						this.labelScore2.Content = String.Format("{0} ({1}:{2})", e.Team.Score, e.Team.Side, e.Team.ScoreFirstHalf);
					}
				}
			}
			));
		}
		#endregion

		private void SortScoreboard()
		{
			CollectionView dataView = (CollectionView)CollectionViewSource.GetDefaultView(this.dataGridScoreboard.ItemsSource);
			//clear the existing sort order
			dataView.SortDescriptions.Clear();
			//create a new sort order for the sorting that is done lastly
			dataView.SortDescriptions.Add(new SortDescription("Rating", ListSortDirection.Descending));
			//refresh the view which in turn refresh the grid
			dataView.Refresh();
		}
	}
}
