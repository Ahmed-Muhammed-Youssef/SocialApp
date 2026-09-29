<#
.SYNOPSIS
    Seeds the SocialApp database at a benchmark scale by calling the tools/SeedDB CLI.

.DESCRIPTION
      Scale      Users    Friends/user  Posts/user   Posts   Purpose
      small          100            99          50     5 k   Fast iteration; matches today's baseline
      medium       1 000           150         100   100 k   Realistic small app
      large       10 000           300         100     1 M   Where the architecture should break
      xlarge     100 000           300         100    10 M   Future growth
      xxlarge  1 000 000           300         100   100 M   Future growth

    The target database must already have its schema, roles and cities: start the API once, or apply
    scripts/seed_essential_data.sql, first.

.EXAMPLE
    ./scripts/seed-bench.ps1 -Scale small
.EXAMPLE
    ./scripts/seed-bench.ps1 -Scale medium -Reset
#>
param(
    [Parameter(Mandatory)]
    [ValidateSet('small', 'medium', 'large', 'xlarge', 'xxlarge')]
    [string] $Scale,

    # Defaults to the docker compose SQL Server (docker compose up -d app-db).
    [string] $Connection = 'Server=localhost,1433;Database=AppDb;User Id=sa;Password=Password123!;TrustServerCertificate=True;',

    # Delete previously seeded rows (the @seed.local ones) before seeding.
    [switch] $Reset
)

$ErrorActionPreference = 'Stop'

$scales = @{
    small   = @{ Users = 100;     FriendsPerUser = 99;  PostsPerUser = 50 }
    medium  = @{ Users = 1000;    FriendsPerUser = 150; PostsPerUser = 100 }
    large   = @{ Users = 10000;   FriendsPerUser = 300; PostsPerUser = 100 }
    xlarge  = @{ Users = 100000;  FriendsPerUser = 300; PostsPerUser = 100 }
    xxlarge = @{ Users = 1000000; FriendsPerUser = 300; PostsPerUser = 100 }
}
$selected = $scales[$Scale]

$resetArgs = @()
if ($Reset) {
    $resetArgs = @('--reset')
}

$seedProject = Join-Path (Split-Path -Parent $PSScriptRoot) 'tools/SeedDB'

& dotnet run -c Release --project $seedProject -- `
    --connection $Connection `
    --users $selected.Users `
    --friends-per-user $selected.FriendsPerUser `
    --posts-per-user $selected.PostsPerUser `
    @resetArgs

# Pass SeedDB's exit code through (2 bad arguments, 3 missing prerequisites, 4 cancelled).
exit $LASTEXITCODE
