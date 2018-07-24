﻿using Avalonia;
using Avalonia.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Shell.Extensibility.Platforms;
using Dock.Model;
using System;
using Avalonia.Threading;
using AvalonStudio.Extensibility.Theme;

namespace AvalonStudio.Shell
{
	public static class Shell
	{
		public static void StartShellApp<TAppBuilder, TMainWindow>(this TAppBuilder builder, string appName, IDockFactory layoutFactory = null, Func<object> dataContextProvider = null) where TAppBuilder : AppBuilderBase<TAppBuilder>, new() where TMainWindow : Window, new()
		{
			builder.UseReactiveUI().AfterSetup(_ =>
			{
				Platform.AppName = appName;
				Platform.Initialise();

				var extensionManager = new ExtensionManager();
				var container = CompositionRoot.CreateContainer(extensionManager);

				IoC.Initialise(container);

				ShellViewModel.Instance = IoC.Get<ShellViewModel>();

				ShellViewModel.Instance.Initialise(layoutFactory);
			}).Start<TMainWindow>(dataContextProvider);
		}
    }
}
