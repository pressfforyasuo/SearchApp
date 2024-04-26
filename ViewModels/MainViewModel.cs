using SearchApp.Commands;
using SearchApp.Models;
using SearchApp.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SearchApp.ViewModels
{
    class MainViewModel
    {
        public MainModel Model { get; set; }

        private DispatcherTimer? timer;
        private DateTime startTime;
        private TimeSpan totalElapsedTime;
        private bool isPaused = false;
        private CancellationTokenSource? cancellationTokenSource;

        public MainViewModel()
        {
            MainModelSerializer serializer = new MainModelSerializer("MainModelData.xml");
            Model = serializer.Deserialize();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (!isPaused && !cancellationTokenSource.Token.IsCancellationRequested)
            {
                TimeSpan elapsedTime = DateTime.Now - startTime + totalElapsedTime;
                Model.TimePassed = string.Format("{0:D2}:{1:D2}:{2:D2}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
            }
        }

        private async Task SearchFilesAsync(string searchDir, string fileNameTemplate)
        {
            Model.SearchButtonIsEnable = false;
            Model.ProgressBarIsIndeterminate = true;
            startTime = DateTime.Now;
            totalElapsedTime = DateTime.Now - startTime;
            timer?.Start();

            isPaused = false;

            if (string.IsNullOrWhiteSpace(searchDir) || string.IsNullOrWhiteSpace(fileNameTemplate))
            {
                return;
            }

            cancellationTokenSource = new CancellationTokenSource();

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(searchDir);
                if (!dirInfo.Exists)
                {
                    MessageBox.Show("Данная директория не обнаружена!", "Ошибка поиска", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Regex regex = new Regex(fileNameTemplate);

                Model.CurDir = dirInfo.FullName;
                Model.RootDirectories?.Clear();
                Model.AllFilesCount = 0;
                Model.FilesFindCount = 0;


                await SearchInDirectoryAsync(dirInfo, regex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}", "Ошибка поиска", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Model.SearchButtonIsEnable = true;
                Model.ProgressBarIsIndeterminate = false;
                timer?.Stop();
            }
        }

        private async Task SearchInDirectoryAsync(DirectoryInfo directory, Regex regex)
        {
            try
            {
                await Task.Run(async() =>
                {
                    foreach (var file in directory.GetFiles())
                    {
                        if (isPaused)
                        {
                            while (isPaused)
                            {
                                await Task.Delay(100);
                                cancellationTokenSource?.Token.ThrowIfCancellationRequested();

                            }
                        }

                        Model.AllFilesCount++;
                        if (regex.IsMatch(file.Name))
                        {
                            Model.FilesFindCount++;
                            string relativePath = file.FullName.Replace(Model.StartDir, "").TrimStart('\\');
                            string[] parts = relativePath.Split('\\');
                            DirectoryNode? currentDir = null;

                            if (Model.RootDirectories?.Count < 1)
                            {
                                Application.Current?.Dispatcher.Invoke(() =>
                                {
                                    Model.RootDirectories?.Add(new DirectoryNode(Model.StartDir.Split("\\")[1]));
                                });
                            }

                            currentDir = Model.RootDirectories?.FirstOrDefault();

                            for (int i = 0; i < parts.Length; i++)
                            {
                                bool isLastPart = i == parts.Length - 1;
                                if (isPaused)
                                {
                                    while (isPaused)
                                    {
                                        await Task.Delay(100);
                                        cancellationTokenSource?.Token.ThrowIfCancellationRequested();
                                    }
                                }
                                DirectoryNode? subDir = currentDir?.Subdirectories?.FirstOrDefault(d => d.Select(d => d.Name == parts[i]).FirstOrDefault())?.FirstOrDefault(); if (subDir == null)
                                {
                                    subDir = new DirectoryNode(parts[i]);
                                    Application.Current?.Dispatcher.Invoke(() =>
                                    {
                                        currentDir?.Subdirectories.Add(new ObservableCollection<DirectoryNode> { subDir });
                                    });
                                }
                                currentDir = subDir;
                            }
                        }
                    }

                    foreach (var subDir in directory.GetDirectories())
                    {
                        Model.CurDir = subDir.FullName;
                        if (isPaused)
                        {
                            while (isPaused)
                            {
                                await Task.Delay(100);
                                cancellationTokenSource?.Token.ThrowIfCancellationRequested();
                            }
                        }
                        await SearchInDirectoryAsync(subDir, regex);
                    }
                });
            }
            catch (OperationCanceledException)
            {
            }
        }

        private ICommand? search;
        public ICommand Search
        {
            get
            {
                return search ?? (search = new RelayCommand(async param =>
                {
                    if (!string.IsNullOrWhiteSpace(Model.StartDir) && !string.IsNullOrWhiteSpace(Model.TemplateName))
                    {
                        if (cancellationTokenSource != null && !cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            cancellationTokenSource.Cancel();
                            await Task.Delay(1000);
                            PauseOrResumeTimer();
                        }

                        await SearchFilesAsync(Model.StartDir, Model.TemplateName);
                    }
                }));
            }
        }

        private ICommand? pause;
        public ICommand Pause
        {
            get
            {
                return pause ?? (pause = new RelayCommand(param =>
                {
                    if (!string.IsNullOrWhiteSpace(Model.StartDir) && !string.IsNullOrWhiteSpace(Model.TemplateName))
                    {
                        isPaused = !isPaused;
                        PauseOrResumeTimer();
                    }
                }));
            }
        }

        private void PauseOrResumeTimer()
        {
            if (isPaused)
            {
                totalElapsedTime += DateTime.Now - startTime;
                Model.SearchButtonIsEnable = true;
                Model.ProgressBarIsIndeterminate = false;
            }
            else
            {
                Model.SearchButtonIsEnable = false;
                Model.ProgressBarIsIndeterminate = true;
                startTime = DateTime.Now;
            }
        }
    }
}
