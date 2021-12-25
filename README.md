# Rambles
A minimal ASP.NET Core website that serves Markdown files with YAML front matter as minimalistic blog pages.

## Pages
A page file has the following format:
```yaml
---
Title: Home
Date: 12/26/2021 # If specified, the page is added to the footer and sorted by this value.
HeaderIndex: 0 # If specified, the page is added to the header and sorted by this value.
---
Markdown-formatted page content
```
Including the YAML front matter is optional.

All pages have the same layout: title caption, header, content, footer.

If a request matches a `.md` file, it is served as a blog page. Otherwise, a static file is served, or a 404 page is diplayed.

## Configuration
 - Pages and static content are served from the working directory.
 - Text above the page header is taken from the first command line argument.
 - All pages reference a stylesheet at `/style.css`.
 
## Why?
I wanted a personal website, but didn't want to get into Hexo. As a result, I learned how to create a website with ASP.NET Core. Yay!
