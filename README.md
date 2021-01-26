# Jamstack on .Net

![Build](https://github.com/Kentico/jamstackon.net/workflows/.NET%20Core/badge.svg)
![Release](https://github.com/Kentico/jamstackon.net/workflows/Publish%20site/badge.svg)
[![Live](https://img.shields.io/badge/Live-DEMO-brightgreen.svg?logo=github&logoColor=white)](https://jamstackon.net)

[![Preview Status](https://api.netlify.com/api/v1/badges/ea4874c1-69a7-4fd6-8109-0457845bf4f0/deploy-status)](https://app.netlify.com/sites/jamstackondotnet/deploys)
[![Live preview](https://img.shields.io/badge/Live-Demo-00C7B7.svg?logo=netlify)](https://jamstackondotnet.netlify.app/)

[![GitHub Discussions](https://img.shields.io/badge/GitHub-Discussions-FE7A16.svg?style=popout&logo=github)](https://github.com/Kentico/Home/discussions)
[![Stack Overflow](https://img.shields.io/badge/Stack%20Overflow-ASK%20NOW-FE7A16.svg?logo=stackoverflow&logoColor=white)](https://stackoverflow.com/tags/kentico-cloud)

<https://jamstackon.net>

Microsite utilizing [Statiq](https://statiq.dev/) and [Kentico Kontent](https://kontent.ai) via [Kontent.Statiq](https://github.com/alanta/Kontent.Statiq) module to evangelize the Jamstack world for .NET developers.

[![Screenshot](./screenshot.png)](https://jamstackon.net)

## Get started

### Requirements

- [.NET 5](https://dotnet.microsoft.com/download)

### Clone the codebase

```sh
git clone https://github.com/Kentico/jamstackon.net
```

### Running locally

```sh
dotnet run -- preview
```

🎊🎉 **Visit <http://localhost:5080> and start exploring the code base!**

> The content is loaded from a Kentico Kontent project. Contact [Kontent DevRel team](mailto:devrel@kentico.com) to contribute to the content itself.

### Build

```sh
dotnet run
```

👀 Checkout the `/output` folder for the static site ready to be deployed.
