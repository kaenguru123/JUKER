using System;
using System.ComponentModel;

public class ViewModelBase : INotifyPropertyChanged
{
        public bool IsSaveable { get; set; }
	public ViewModelBase()
	{
            IsSaveable = true;
	}

    public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
