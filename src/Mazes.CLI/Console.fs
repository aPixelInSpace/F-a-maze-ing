module Mazes.CLI.Console

open Spectre.Console

let printSuccess message =
    AnsiConsole.MarkupLine($"[bold springgreen2_1]{message}[/]")

let printProgress message =
    AnsiConsole.MarkupLine($"[bold orange1]{message}[/]")

let printError message =
    AnsiConsole.MarkupLine($"[bold red]{message}[/]")