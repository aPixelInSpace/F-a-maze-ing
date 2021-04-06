module Mazes.CLI.Console

open Spectre.Console

let printSuccess message =
    AnsiConsole.Markup($"[bold springgreen2_1]{message}[/]")

let printSuccessN message =
    AnsiConsole.MarkupLine($"[bold springgreen2_1]{message}[/]")

let printProgress message =
    AnsiConsole.Markup($"[bold orange1]{message}[/]")

let printProgressN message =
    AnsiConsole.MarkupLine($"[bold orange1]{message}[/]")

let printError message =
    AnsiConsole.Markup($"[bold red]{message}[/]")

let printErrorN message =
    AnsiConsole.MarkupLine($"[bold red]{message}[/]")