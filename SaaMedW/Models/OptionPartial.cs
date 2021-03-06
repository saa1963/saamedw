﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class OptionType
    {
        public Type type { get; set; }
        public enumProfile profile { get; set; }
        public object defaultValue { get; set; }
        public bool IsEditable { get; set; }
    }
    public partial class Options : NotifyPropertyChanged
    {
        public static Dictionary<enumParameterType, OptionType> ВсеВидыПараметров { get; set; } =
            new Dictionary<enumParameterType, OptionType>()
            {
                { enumParameterType.Наименование_организации,
                    new OptionType()
                    {
                        type = typeof(string), profile = enumProfile.Общий, defaultValue = "", IsEditable = true
                    }
                },
                {enumParameterType.Коэффициент_для_Excel,
                    new OptionType()
                    { type = typeof(int), profile = enumProfile.ЛокальныйВсеПользователи, defaultValue = 88, IsEditable = true }
                },
                {enumParameterType.Настройки_ФР,
                    new OptionType()
                    { type = typeof(string), profile = enumProfile.ЛокальныйВсеПользователи, defaultValue = null, IsEditable = true }
                },
                {enumParameterType.Система_налогообложения,
                    new OptionType()
                    { type = typeof(enTaxSystem), profile = enumProfile.Общий, defaultValue = enTaxSystem.Общая, IsEditable = true }
                },
                {enumParameterType.НДС,
                    new OptionType()
                    { type = typeof(enNds), profile = enumProfile.Общий, defaultValue = enNds.Процент_20, IsEditable = true }
                },
                {enumParameterType.Последний_логин,
                    new OptionType()
                    { type = typeof(string), profile = enumProfile.ЛокальныйВсеПользователи, defaultValue = "", IsEditable = false}
                },
                {enumParameterType.Номер_договора,
                    new OptionType()
                    { type = typeof(int), profile = enumProfile.Общий, defaultValue = 1, IsEditable = true}
                },
                {enumParameterType.Лицензия,
                    new OptionType()
                    { type = typeof(string), profile = enumProfile.Общий, defaultValue = "", IsEditable = true}
                },
                {enumParameterType.ФИО_руководителя,
                    new OptionType()
                    { type = typeof(string), profile = enumProfile.Общий, defaultValue = "", IsEditable = true}
                },
                {enumParameterType.Юридический_адрес,
                    new OptionType()
                    { type = typeof(string), profile = enumProfile.Общий, defaultValue = "", IsEditable = true}
                },
                {enumParameterType.Телефоны,
                    new OptionType()
                    { type = typeof(string), profile = enumProfile.Общий, defaultValue = "", IsEditable = true}
                },
                {enumParameterType.ИНН,
                    new OptionType()
                    { type = typeof(string), profile = enumProfile.Общий, defaultValue = "", IsEditable = true}
                },
                {enumParameterType.КПП,
                    new OptionType()
                    { type = typeof(string), profile = enumProfile.Общий, defaultValue = "", IsEditable = true}
                },
                {enumParameterType.БИК_банка,
                    new OptionType()
                    { type = typeof(string), profile = enumProfile.Общий, defaultValue = "", IsEditable = true}
                },
                {enumParameterType.Наименование_банка,
                    new OptionType()
                    { type = typeof(string), profile = enumProfile.Общий, defaultValue = "", IsEditable = true}
                },
                {enumParameterType.Расчетный_счет,
                    new OptionType()
                    { type = typeof(string), profile = enumProfile.Общий, defaultValue = "", IsEditable = true}
                }
            };
        public string Name
        {
            get => Enum.GetName(typeof(enumParameterType), ParameterType).Replace('_', ' ');
        }
        public List<object> EnumList
        {
            get
            {
                var tp = Options.ВсеВидыПараметров[this.ParameterType].type;
                if (tp.IsSubclassOf(typeof(Enum)))
                {
                    var rt = new List<object>();
                    foreach (var o in Enum.GetValues(tp))
                    {
                        rt.Add(o);
                    }
                    return rt;
                }
                else
                    return null;
            }
        }

        //public delegate void DValueChanged();
        //public event DValueChanged ValueChanged;

        public Enum SelectedEnum
        {
            get
            {
                return (Enum)GetObject();
            }
            set
            {
                SetObject(value);
                OnPropertyChanged("ParameterValue");
                //ValueChanged?.Invoke();
            }
        }

        /// <summary>
        /// Возвращает из БД указанный параметр или значение по умолчанию
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">Тип параметра</param>
        /// <returns></returns>
        public static T GetParameter<T>(enumParameterType type)
        {
            string compId = "0";
            int userId = 0;
            switch (ВсеВидыПараметров[type].profile)
            {
                case enumProfile.Общий:
                    compId = "0";
                    userId = 0;
                    break;
                case enumProfile.ЛокальныйПользователя:
                    compId = Global.Source.GetMotherboardId();
                    userId = Global.Source.RUser.Id;
                    break;
                case enumProfile.ЛокальныйВсеПользователи:
                    compId = Global.Source.GetMotherboardId();
                    userId = 0;
                    break;
                case enumProfile.ПеремещаемыйПользователя:
                    compId = "0";
                    userId = Global.Source.RUser.Id;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            using (var ctx = new SaaMedEntities())
            {

                var opt = ctx.Options.Find(new object[] { type, userId, compId });
                if (opt != null)
                    return opt.GetObject<T>();
                else
                    return (T)Convert.ChangeType(ВсеВидыПараметров[type].defaultValue, typeof(T));
            }
        }

        /// <summary>
        /// Возвращает значение параметра
        /// </summary>
        /// <returns></returns>
        public object GetObject()
        {
            Type type = ВсеВидыПараметров[this.ParameterType].type;
            if (type == typeof(string))
                if (ParameterValue != "NULL")
                    return ParameterValue;
                else
                    return null;
            else if (type == typeof(System.IO.Path))
                return ParameterValue;
            else if (type == typeof(int))
                if (Int32.TryParse(ParameterValue, out int resultInt))
                    return resultInt;
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return null;
                }
            else if (type == typeof(decimal))
                if (Decimal.TryParse(ParameterValue, out decimal resultDecimal))
                    return resultDecimal;
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return null;
                }
            else if (type == typeof(DateTime))
            {
                var f = new CultureInfo("ru-RU").DateTimeFormat;
                if (DateTime.TryParse(ParameterValue, f, DateTimeStyles.None, out DateTime resultDateTime))
                    return resultDateTime;
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return null;
                }
            }
            else if (type.IsSubclassOf(typeof(Enum)))
            {

                if (Int32.TryParse(ParameterValue, out int resultInt))
                {
                    try
                    {
                        return Enum.ToObject(type, resultInt);
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return null;
                }
            }
            else
            {
                Debug.Assert(false, "Недопустимый тип параметра.");
                return null;
            }
        }
        public T GetObject<T>()
        {
            Type type = ВсеВидыПараметров[this.ParameterType].type;
            if (type == typeof(string))
                if (ParameterValue != "NULL")
                    return (T)Convert.ChangeType(ParameterValue, typeof(T));
                else
                    return default(T);
            else if (type == typeof(System.IO.Path))
                return (T)Convert.ChangeType(ParameterValue, typeof(T));
            else if (type == typeof(int))
                if (Int32.TryParse(ParameterValue, out int resultInt))
                    return (T)Convert.ChangeType(resultInt, typeof(T));
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return default(T);
                }
            else if (type == typeof(decimal))
                if (Decimal.TryParse(ParameterValue, out decimal resultDecimal))
                    return (T)Convert.ChangeType(resultDecimal, typeof(T));
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return default(T);
                }
            else if (type == typeof(DateTime))
            {
                var f = new CultureInfo("ru-RU").DateTimeFormat;
                if (DateTime.TryParse(ParameterValue, f, DateTimeStyles.None, out DateTime resultDateTime))
                    return (T)Convert.ChangeType(resultDateTime, typeof(T));
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return default(T);
                }
            }
            else if (type.IsSubclassOf(typeof(Enum)))
            {
                if (Int32.TryParse(ParameterValue, out int resultInt))
                {
                    try
                    {
                        return (T)Enum.ToObject(type, resultInt);
                    }
                    catch
                    {
                        return default(T);
                    }
                }
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return default(T);
                }
            }
            else
            {
                Debug.Assert(false, "Недопустимый тип параметра.");
                return default(T);
            }
        }
        public void SetObject(object Value)
        {
            object dv = ВсеВидыПараметров[this.ParameterType].defaultValue;

            if (Value == null)
                dv = ВсеВидыПараметров[this.ParameterType].defaultValue;
            else
                dv = Value;
            if (Value is string)
                ParameterValue = dv.ToString();
            else if (Value is System.IO.Path)
                ParameterValue = dv.ToString();
            else if (Value is int)
                ParameterValue = ((int)dv).ToString();
            else if (Value is decimal)
                ParameterValue = ((decimal)dv).ToString();
            else if (Value is DateTime)
            {
                ParameterValue = ((DateTime)dv).ToString(new CultureInfo("ru-RU").DateTimeFormat);
            }
            else if (Value is Enum)
            {
                ParameterValue = ((int)dv).ToString();
            }
            else if (Value == null && dv == null)
            {
                ParameterValue = "NULL";
            }
            else
            {
                Debug.Assert(false, message: $"Недопустимый тип параметра. Value = {this.Name}");
            }
        }
        public static void SetParameter<T>(enumParameterType type, T value)
        {
            string compId = "0";
            int userId = 0;
            switch (ВсеВидыПараметров[type].profile)
            {
                case enumProfile.Общий:
                    compId = "0";
                    userId = 0;
                    break;
                case enumProfile.ЛокальныйПользователя:
                    compId = Global.Source.GetMotherboardId();
                    userId = Global.Source.RUser.Id;
                    break;
                case enumProfile.ЛокальныйВсеПользователи:
                    compId = Global.Source.GetMotherboardId();
                    userId = 0;
                    break;
                case enumProfile.ПеремещаемыйПользователя:
                    compId = "0";
                    userId = Global.Source.RUser.Id;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            using (var ctx = new SaaMedEntities())
            {

                var opt = ctx.Options.Find(new object[] { type, userId, compId });
                if (opt == null)
                {
                    opt = new Options() { CompId = compId, UserId = userId, ParameterType = type };
                    ctx.Options.Add(opt);
                }
                opt.SetObject(value);
                ctx.SaveChanges();
            }
        }
    }
}
