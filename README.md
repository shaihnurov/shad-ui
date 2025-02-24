# ShadUI ✨

ShadUI is an **Avalonia-based Desktop UI Library** inspired by [shadcn/ui](https://ui.shadcn.com/)
and [Suki UI Library](https://kikipoulet.github.io/SukiUI/).  
Our goal is to deliver a modern, beautiful, and intuitive UI library for [Avalonia](https://avaloniaui.net/).

[![NuGet](https://img.shields.io/nuget/v/ShadUI.svg)](https://www.nuget.org/packages/ShadUI)
[![NuGet Downloads](https://img.shields.io/nuget/dt/ShadUI)](https://www.nuget.org/packages/ShadUI)
[![GitHub stars](https://img.shields.io/github/stars/accntech/shad-ui)](https://github.com/accntech/shad-ui/stargazers)

**Forever free and open-source.** 🚀

![Hero Image](https://raw.githubusercontent.com/accntech/shad-ui/main/docs/hero.png)

---

## Installation 🚀

To get started with ShadUI:

Install the [ShadUI NuGet package](https://www.nuget.org/packages/ShadUI/):

```powershell
dotnet add package ShadUI
```

> Note: The command above will install the latest version automatically. You can also specify a version if you want to install a specific version or install a preview version.

```powershell
dotnet add package ShadUI --version [version]
```

Include ShadTheme in your App.xaml:

```xml
<Application
    xmlns:themes="clr-namespace:ShadUI.Themes;assembly=ShadUI">

    <!-- other code -->

    <Application.Styles>
        <themes:ShadTheme />
        <!-- other styles -->
    </Application.Styles>
</Application>
```

> Recommended: Use the `ShadUI.Controls.Window` instead of `Avalonia.Controls.Window` to get the full ShadUI experience.
 
Install the [ShadUI App](https://apps.microsoft.com/detail/9N3358B0PHG4?hl=en-us&gl=PH&ocid=pdpshare) from Microsoft Store for examples of how to use ShadUI controls.

## Features 🌟

### 🎨 Theme

ShadUI provides a default theme out of the box, allowing you to get started quickly with a professional look and feel.  
Dynamic theming is currently under development and will be available soon.

![Theme Demo](https://raw.githubusercontent.com/accntech/shad-ui/main/docs/demo-01.gif)

---

### 🛠️ Controls

ShadUI includes a growing set of essential UI controls, inspired by [shadcn/ui](https://ui.shadcn.com/)
and [Suki UI Library](https://kikipoulet.github.io/SukiUI/).  
We are actively working to expand the library with additional controls and advanced features.

![Controls Demo](https://raw.githubusercontent.com/accntech/shad-ui/main/docs/demo-02.gif)

---

### 💬 Dialogs

ShadUI offers a simple yet powerful system for dialogs, enabling you to create interactive and responsive user
experiences effortlessly.

![Dialogs Demo](https://raw.githubusercontent.com/accntech/shad-ui/main/docs/demo-03.gif)

---

### 🔔 Toasts

Deliver quick, non-intrusive messages with ShadUI's built-in toast notifications system, designed for clarity and ease
of use.

![Toasts Demo](https://raw.githubusercontent.com/accntech/shad-ui/main/docs/demo-04.gif)

---

## Acknowledgments 💖

ShadUI wouldn't be possible without the inspiration and contributions of the following projects and libraries:

- [Avalonia](https://avaloniaui.net/): A cross-platform XAML-based UI framework with a robust styling system.
- [shadcn/ui](https://ui.shadcn.com/) and [Suki UI Library](https://kikipoulet.github.io/SukiUI/): The foundation of our
  design principles and control ideas.
- [Lucide Avalonia Icons](https://github.com/MarwanFr/LucideAvaloniaUI/): A collection of icons tailored for Avalonia
  UI.
- [Responsive.Avalonia](https://github.com/russkyc/responsive-avalonia): A library for building responsive layouts.
- [LiveCharts](https://livecharts.dev/): A library for creating visually stunning charts.
- [MvvmToolkit](https://github.com/CommunityToolkit): A framework for implementing the MVVM pattern efficiently.

---

## Contributing 🤝

We welcome contributions to ShadUI! Before getting started, please review our [Contributing Guidelines](https://github.com/accntech/shad-ui/blob/main/CONTRIBUTING.md) and [Code of Conduct](https://github.com/accntech/shad-ui/blob/main/CODE_OF_CONDUCT.md).  
Feel free to open issues, suggest new features, or submit pull requests.

---

## License 📜

ShadUI is licensed under the **MIT License**. See the [LICENSE](https://github.com/accntech/shad-ui/blob/main/LICENSE) file for details.
