# Rambles
A minimal ASP.NET Core website that serves Markdown files with YAML front matter as minimalistic blog pages.

## Pages
A page file has the following format:
```yaml
---
Title: Home
Date: 12/26/2021 # If specified, the page is added to the footer and sorted by this value.
HeaderIndex: 0 # If specified, the page is added to the header and sorted by this value.
HideFooter: true # If explicitly set to true, removes the footer from the page.
---
Markdown-formatted page content
```
Including the YAML front matter is optional.

All pages have the same layout: title caption, header, content, footer.

If a request matches a `.md` file, it is served as a blog page. Otherwise, a static file is served, or a 404 page is diplayed.

A sitemap containing all pages with either `Date` or `HeaderIndex` set is available at `/sitemap.xml`.  
`lastmod` value is set to page file's last write date.

## Configuration
Settings are supplied via a config file specified as the single command line argument (`rambles.cfg` by default). The format is as follows:
```yaml
WebsiteName: Hello world! # Displayed at the top of every page.
BaseUrl: https://example.com/ # Used to build the sitemap.
RootDirectory: /home/pi/html # Used to locate Markdown files and static files.
```
 
## Why?
I wanted a personal website, but didn't want to get into Hexo. As a result, I learned how to create a website with ASP.NET Core. Yay!
