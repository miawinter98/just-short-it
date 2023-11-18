# Changelog

## [unreleased]

### Added
- new css style, created using tailwindcss and daisyui

### Fixed
- form submit bug on Urls page, that happens when one first tries to look up an ID unsuccessfully, and then tries to create a new redirect

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