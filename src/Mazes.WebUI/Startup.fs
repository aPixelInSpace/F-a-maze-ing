// Copyright 2020-2021 Patrizio Amella. All rights reserved. See License file in the project root for more information.

namespace Mazes.WebUI.Client

open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Bolero.Remoting.Client
open Blazorise
open Blazorise.Bootstrap
open Blazorise.Icons.FontAwesome

module Program =

    [<EntryPoint>]
    let Main args =
        let builder = WebAssemblyHostBuilder.CreateDefault(args)
        builder.RootComponents.Add<Main.MyApp>("#main")
        builder.Services.AddRemoting(builder.HostEnvironment) |> ignore
        builder.Services
            .AddBlazorise(fun opt -> opt.ChangeTextOnKeyPress <- true)
            .AddBootstrapProviders()
            .AddFontAwesomeIcons()
            |> ignore

        let host = builder.Build()

        host.RunAsync() |> ignore
        0
