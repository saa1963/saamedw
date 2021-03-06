﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaaMedW
{
    public class OptionsViewModel: NotifyPropertyChanged
    {
        private readonly SaaMedEntities ctx = new SaaMedEntities();
        private ObservableCollection<Options> m_CommonParameterList
            = new ObservableCollection<Options>();
        private ObservableCollection<Options> m_UserParameterList
            = new ObservableCollection<Options>();
        private ObservableCollection<Options> m_ComputerParameterList
            = new ObservableCollection<Options>();
        private ObservableCollection<Options> m_UserComputerParameterList
            = new ObservableCollection<Options>();
        public ObservableCollection<Options> CommonParameterList
        {
            get => m_CommonParameterList;
            set
            {
                m_CommonParameterList = value;
                OnPropertyChanged("CommonParameterList");
            }
        }
        public ObservableCollection<Options> UserParameterList
        {
            get => m_UserParameterList;
            set
            {
                m_UserParameterList = value;
                OnPropertyChanged("UserParameterList");
            }
        }
        public ObservableCollection<Options> ComputerParameterList
        {
            get => m_ComputerParameterList;
            set
            {
                m_ComputerParameterList = value;
                OnPropertyChanged("ComputerParameterList");
            }
        }
        public ObservableCollection<Options> UserComputerParameterList
        {
            get => m_UserComputerParameterList;
            set
            {
                m_UserComputerParameterList = value;
                OnPropertyChanged("UserComputerParameterList");
            }
        }
        private bool m_IsChanged = false;
        public bool IsChanged
        {
            get => m_IsChanged;
            set
            {
                m_IsChanged = value;
                OnPropertyChanged("IsChanged");
            }
        }
        public OptionsViewModel()
        {
            var compId = Global.Source.GetMotherboardId();
            // Общие настройки
            if (Global.Source.RUser.Role == 0)
            {
                foreach (var o in Options.ВсеВидыПараметров.Where(s => s.Value.profile == enumProfile.Общий && s.Value.IsEditable))
                {
                    var nv = new Options()
                    {
                        ParameterType = o.Key,
                        Profile = enumProfile.Общий,
                        CompId = "0",
                        UserId = 0
                    };
                    nv.PropertyChanged += Nv_PropertyChanged;
                    nv.SetObject(Options.GetParameter<object>(o.Key));
                    m_CommonParameterList.Add(nv);
                }
            }
            // Перемещаемые настройки пользователя
            foreach (var o in Options.ВсеВидыПараметров.Where(s => s.Value.profile == enumProfile.ПеремещаемыйПользователя && s.Value.IsEditable))
            {
                var nv = new Options()
                {
                    ParameterType = o.Key,
                    Profile = enumProfile.ПеремещаемыйПользователя,
                    CompId = "0",
                    UserId = Global.Source.RUser.Id
                };
                nv.PropertyChanged += Nv_PropertyChanged;
                nv.SetObject(Options.GetParameter<object>(o.Key));
                m_UserParameterList.Add(nv);
            }
            // Локальные настройки для всех пользователей
            if (Global.Source.RUser.Role == 0)
            {
                foreach (var o in Options.ВсеВидыПараметров.Where(s => s.Value.profile == enumProfile.ЛокальныйВсеПользователи && s.Value.IsEditable))
                {
                    var nv = new Options()
                    {
                        ParameterType = o.Key,
                        Profile = enumProfile.ЛокальныйВсеПользователи,
                        CompId = compId,
                        UserId = 0
                    };
                    nv.PropertyChanged += Nv_PropertyChanged;
                    nv.SetObject(Options.GetParameter<object>(o.Key));
                    m_ComputerParameterList.Add(nv);
                }
            }
            // Локальные настройки пользователя
            foreach (var o in Options.ВсеВидыПараметров.Where(s => s.Value.profile == enumProfile.ЛокальныйПользователя && s.Value.IsEditable))
            {
                var nv = new Options()
                {
                    ParameterType = o.Key,
                    Profile = enumProfile.ЛокальныйПользователя,
                    CompId = compId,
                    UserId = Global.Source.RUser.Id
                };
                nv.PropertyChanged += Nv_PropertyChanged;
                nv.SetObject(Options.GetParameter<object>(o.Key));
                m_UserComputerParameterList.Add(nv);
            }
        }

        private void Nv_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsChanged = true;
        }

        public RelayCommand SaveCommand
        {
            get => new RelayCommand(SaveParameters);
        }

        private void SaveParameters(object obj)
        {
            var mas = new ObservableCollection<Options>[]
                { CommonParameterList, ComputerParameterList, UserParameterList, UserComputerParameterList };
            foreach (var m in mas)
            {
                foreach (var o in m)
                {
                    var opt = ctx.Options.Find(new object[] { o.ParameterType, o.UserId, o.CompId });
                    if (opt != null)
                    {
                        opt.ParameterValue = o.ParameterValue;
                    }
                    else
                    {
                        ctx.Options.Add(o);
                    }
                }
            }
            ctx.SaveChanges();
            MessageBox.Show("Параметры сохранены");
            IsChanged = false;
        }

        public RelayCommand EditMultiLineCommand => new RelayCommand(EditMultiLine);

        private void EditMultiLine(object obj)
        {
            var s = obj as Options;
            var modelView = new MultiEditViewModel() { Text = String.Copy(s.ParameterValue) };
            var f = new MultiEditView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                s.ParameterValue = modelView.Text;
                IsChanged = true;
                s.OnPropertyChanged("ParameterValue");
            }
        }
    }
}
