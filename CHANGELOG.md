# Changelog

## [1.3.0] - 2023-11-18

### Added
- new css style, created using tailwindcss and daisyui
- new favicon (replacing the default asp.net one)
- about page (linked in footer)

### Changed 
- migrated app to use blazor static site rendering for all pages

### Removed
- bulma based css style

## [1.2.0] - 2023-11-17

### Added

- Page Titles to Login, Logout and Urls Page
- Added Inspect Page, which displays information about a URL and allows to delete it
- Added Inspect Form group to Urls that redirects to Inspect Page
- Added Docker-Multiplatform support (future images will contain linux/amd64 and linux/arm64 images)
- Added alpine images

## Changed

- Migrated project to .NET 8
- Container now uses Port 8080
- Input fields now have autocomplete attributes that improve browser behavior

## [1.1.0] - 2023-04-17

### Changed

- Restyled App with new Theme