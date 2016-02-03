<?php
/**
 * The base configurations of the WordPress.
 *
 * This file has the following configurations: MySQL settings, Table Prefix,
 * Secret Keys, WordPress Language, and ABSPATH. You can find more information
 * by visiting {@link http://codex.wordpress.org/Editing_wp-config.php Editing
 * wp-config.php} Codex page. You can get the MySQL settings from your web host.
 *
 * This file is used by the wp-config.php creation script during the
 * installation. You don't have to use the web site, you can just copy this file
 * to "wp-config.php" and fill in the values.
 *
 * @package WordPress
 */

// ** MySQL settings - You can get this info from your web host ** //
/** The name of the database for WordPress */
define('DB_NAME', 'db556984957');

/** MySQL database username */
define('DB_USER', 'dbo556984957');

/** MySQL database password */
define('DB_PASSWORD', 'ybYmOVBuVofribEkRgFp');

/** MySQL hostname */
define('DB_HOST', 'db556984957.db.1and1.com:3306');

/** Database Charset to use in creating database tables. */
define('DB_CHARSET', 'utf8');

/** The Database Collate type. Don't change this if in doubt. */
define('DB_COLLATE', '');

/**#@+
 * Authentication Unique Keys and Salts.
 *
 * Change these to different unique phrases!
 * You can generate these using the {@link https://api.wordpress.org/secret-key/1.1/salt/ WordPress.org secret-key service}
 * You can change these at any point in time to invalidate all existing cookies. This will force all users to have to log in again.
 *
 * @since 2.6.0
 */
define('AUTH_KEY',         '*X$DnQ1G59L$ZxRlxy9rf6LMtfb^mDqAGwb4&4ICHuNfDhi4A)(ZZMfGCqQV%yMf');
define('SECURE_AUTH_KEY',  '64G#uyq^qz8&FipSgFB#sYgzQJ2W@uD%ALyfvWPxrukGoJHE0Gno2mt%mEg&wh*b');
define('LOGGED_IN_KEY',    'upPqyVCzcUQMetunfuQYa28Hd)idd9pbR5q7k^#fulY0DmZANH4K@1&ayMb1ZfyR');
define('NONCE_KEY',        'YqN%Kr#k!)gz(bh0M^tyCKk3z^)hDT4u*rx14GXikKWo1nV0CpAWmuhplUa38Vq5');
define('AUTH_SALT',        '1oeOJ61Q7VUH4e4pgBExObAhEWN5@gqbAO%2xfwxJwvsGdoNCWLxky7$vkfcKZPx');
define('SECURE_AUTH_SALT', 'uL^PUzgB03b!g(YU8G*0dOvXm(t&snu(Xe!)q%(nY0dKcmbfq^YLbpQQBBcAwfBP');
define('LOGGED_IN_SALT',   'YesrTD7QZPr#$C)1lXAtCFL1VzZNd7q$Zjkn4JFzO@YkcXQ$gN(FVLY8cAvG$mdh');
define('NONCE_SALT',       '7MK&VI1Ah)G5grLgBAE8sg)j#J5aq$tz0EJy)NrlUP#WI8GEZnxw1Fx&WO6#$6J!');

/**#@-*/

/**
 * WordPress Database Table prefix.
 *
 * You can have multiple installations in one database if you give each a unique
 * prefix. Only numbers, letters, and underscores please!
 */
$table_prefix  = 'NAGAZsEY';

/**
 * For developers: WordPress debugging mode.
 *
 * Change this to true to enable the display of notices during development.
 * It is strongly recommended that plugin and theme developers use WP_DEBUG
 * in their development environments.
 */
define('WP_DEBUG', false);

/**
 * Disable the Plugin and Theme Editor.
 *
 * Occasionally you may wish to disable the plugin or theme editor to prevent
 * overzealous users from being able to edit sensitive files and potentially crash the site.
 * Disabling these also provides an additional layer of security if a hacker
 * gains access to a well-privileged user account.
 */
define('DISALLOW_FILE_EDIT', true);

/* That's all, stop editing! Happy blogging. */

/** Absolute path to the WordPress directory. */
if ( !defined('ABSPATH') )
	define('ABSPATH', dirname(__FILE__) . '/');

/** Sets up WordPress vars and included files. */
require_once(ABSPATH . 'wp-settings.php');
