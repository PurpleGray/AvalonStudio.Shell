using ReactiveUI;
using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;

namespace AvalonStudio.Extensibility.Dialogs
{
	public class ModalDialogViewModelBase : ReactiveObject
	{
		private bool cancelButtonVisible;

		private bool isVisible;

		private bool okayButtonVisible;

		private string title;

	    private WindowIcon icon;

        private TaskCompletionSource<bool> dialogCloseCompletionSource;

		public ModalDialogViewModelBase(string title, bool okayButton = true, bool cancelButton = true, string iconUri = null)
		{
			OKButtonVisible = okayButton;
			CancelButtonVisible = cancelButton;

			isVisible = false;
			this.title = title;

			CancelCommand = ReactiveCommand.Create(() => Close(false));

		    if (!string.IsNullOrEmpty(iconUri))
		    {
		        var loader = AvaloniaLocator.Current.GetService<IAssetLoader>();
                var iconStream = loader.Open(new Uri(iconUri));
		        this.icon = new WindowIcon(iconStream);
		    }
        }

		public bool CancelButtonVisible
		{
			get { return cancelButtonVisible; }
			set { this.RaiseAndSetIfChanged(ref cancelButtonVisible, value); }
		}

		public bool OKButtonVisible
		{
			get { return okayButtonVisible; }
			set { this.RaiseAndSetIfChanged(ref okayButtonVisible, value); }
		}

	    public virtual ReactiveCommand OKCommand { get; protected set; }
        public ReactiveCommand CancelCommand { get; }

		public string Title
		{
			get { return title; }
			private set { this.RaiseAndSetIfChanged(ref title, value); }
		}

	    public WindowIcon Icon
	    {
	        get { return icon; }
	        private set { this.RaiseAndSetIfChanged(ref icon, value); }
	    }

        public bool IsVisible
		{
			get { return isVisible; }
			set { this.RaiseAndSetIfChanged(ref isVisible, value); }
		}

        public Task<bool> ShowDialogAsync()
		{
			IsVisible = true;

			dialogCloseCompletionSource = new TaskCompletionSource<bool>();

			return dialogCloseCompletionSource.Task;
		}

		public void Close(bool success = true)
		{
			IsVisible = false;

			dialogCloseCompletionSource.SetResult(success);
		}
	}
}
