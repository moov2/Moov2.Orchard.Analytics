# Moov2.Orchard.Analytics

[Orchard](http://www.orchardproject.net/) module providing analytics on user website usage.

## Status

*Currently under development.*

## Getting Set Up

Download module source code and place within the "Modules" directory of your Orchard installation.

Alternatively, use the command below to add this module as a sub-module within your Orchard project.

    git submodule add git@github.com:moov2/Moov2.Orchard.Analytics.git modules/Moov2.Orchard.Analytics

## Configuration

Once this module is visible within the admin modules section (type "Analytics" in the search box) go ahead and enable the module. To start recording data, go to the "Widgets" section and create a new "Analytics Widgets". In most cases, you'll want to capture data for every page to add the widget to a layer that ensures the widget is rendered on every page (layer rule should be `true`). Alternatively, if you only wish to capture data in specific scenarios, use a custom layer to do this. The Analytics Widget will handle sending data to a custom route that will handle storing in the database.

Once the widget is setup, data will start being captured and viewable within the "Analytics" section in the admin area (`/Admin/Analytics`).