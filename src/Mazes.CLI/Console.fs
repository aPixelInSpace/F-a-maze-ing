module Mazes.CLI.Console

open Spectre.Console

let printSuccess message =
    AnsiConsole.MarkupLine($"[bold green]{message}[/]")

let printProgress message =
    AnsiConsole.MarkupLine($"[bold darkorange]{message}[/]")

let printError message =
    AnsiConsole.MarkupLine($"[bold red]{message}[/]")