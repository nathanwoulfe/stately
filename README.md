# Stately
##Page-state icons for Umbraco 7+

Long-time fans of Umbraco will be familiar with the [Page State Icons](https://bitbucket.org/umbristol/page-state-icons/src/dcf93bbaddd6?at=default) package - Stately is a v7 implementation of the same functionality.

Stately adds a new dashboard to the settings section of your Umbraco installation.

From the dashboard, you can configure rules for displaying page-state icons - set a property alias, true/false and choose an icon. 

The settings are stored in a config file that comes preloaded with best-practice Umbraco properties, being umbracoNaviHide, umbracoRedirect and umbracoUrlAlias.

Stately will match on either the presence of a property value (ie bodyText[true] will add an icon to any node where the bodyText property contains any value) or the presence of a true/false value (ie umbracoNaviHide[false] will add an icon to any node where umbracoNaviHide is set to false).

Stately will only match once per node - it iterates the config values and applies the first match. Given that, the order of the properties in the dashboard can be used to prioritise particular icons.

Umbraco's default page-state indicators take precedence over Stately - the friendly has-unpublish-version class will always be displayed ahead of any Stately defined icons.


