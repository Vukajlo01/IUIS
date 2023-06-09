using NetworkService.Model;
using NetworkService.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NetworkService
{
    public partial class TerminalControl : UserControl
    {
        public ObservableCollection<Entity> Entities { get; set; }

        private List<string> commandHistory;
        private int historyIndex;

        public TerminalControl()
        {
            InitializeComponent();

            Entities = new ObservableCollection<Entity>();

            commandHistory = new List<string>();
            historyIndex = -1;
        }

        private void Prompt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                historyIndex++;
                historyIndex = Math.Min(historyIndex, commandHistory.Count - 1);
                Prompt.Text = commandHistory[commandHistory.Count - historyIndex - 1].ToString();
            }
            else if (e.Key == Key.Down)
            {
                historyIndex--;
                historyIndex = Math.Max(historyIndex, 0);
                Prompt.Text = commandHistory[commandHistory.Count - historyIndex - 1].ToString();
            }
            else if (e.Key == Key.Return)
            {
                commandHistory.Add(Prompt.Text);
                Terminal.Text += "> " + Prompt.Text + "\n";
                if (Prompt.Text.StartsWith("list"))
                {
                    foreach (Entity entity in Entities)
                    {
                        Terminal.Text += entity.ToString() + "\n";
                    }
                }
                else if (Prompt.Text.StartsWith("add"))
                {
                    var words = Prompt.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length == 4)
                    {
                        if (int.TryParse(words[1], out _))
                        {
                            Entity entity = new Entity();
                            entity.Id = int.Parse(words[1]);
                            entity.Name = words[2];
                            entity.Type = new EntityType();
                            entity.Type.Name = words[3];
                            Entities.Add(entity);
                        }
                    }
                }
                else if (Prompt.Text.StartsWith("del"))
                {
                    var words = Prompt.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length == 2)
                    {
                        if (int.TryParse(words[1], out _))
                        {
                            var toDelete = new List<Entity>();
                            foreach (Entity entity in Entities)
                            {
                                if (entity.Id == int.Parse(words[1]))
                                {
                                    toDelete.Add(entity);
                                }
                            }

                            foreach (Entity entity in toDelete)
                            {
                                Entities.Remove(entity);
                            }
                        }
                    }
                }
                Prompt.Text = "";
            }
        }
    }
}