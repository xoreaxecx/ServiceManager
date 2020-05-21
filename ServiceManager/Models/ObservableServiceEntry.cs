using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.Models
{
    public class ObservableServiceEntry : ObservableObject
    {
        private string _name;
        private string _description;
        private string _statusString;
        private string _group;
        private string _path;
        private string _pid;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        public string StatusString
        {
            get { return _statusString; }
            set
            {
                if (value != _statusString)
                {
                    _statusString = value;
                    OnPropertyChanged("StatusString");
                }
            }
        }

        public string Group
        {
            get { return _group; }
            set
            {
                if (value != _group)
                {
                    _group = value;
                    OnPropertyChanged("Group");
                }
            }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                if (value != _path)
                {
                    _path = value;
                    OnPropertyChanged("Path");
                }
            }
        }

        public string PID
        {
            get { return _pid; }
            set
            {
                if (value != _pid)
                {
                    _pid = value;
                    OnPropertyChanged("PID");
                }
            }
        }
    }
}
