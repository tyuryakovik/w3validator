﻿W3C site validator
Используя карту сайта отправляет все страницы на проверку валидности.

Для запуска необходимо разместить в одной директории файлы:
- W3Validity.exe
- Common.dll
- options.cfg

1 - Для работы через прокси сервер нужно указать в options.cfg настройки прокси
2 - При работе с медленных или нестабильных сетей, например по GPRS, можно увеличить таймаут и количество повторных попыток
3 - Для корректной работы необходимо:
    - в корне сайта должен быть размещён robots.txt с директивой Sitemap: /*http://путь к карте сайта*/
    - при отсутствии robots.txt или отсутствии в нем директивы Sitemap: -  карта сайта берётся из корня сайта http://site.ru/sitemap.xml
4 - Если на сайте присутствует sitemap.php и другие разновидности автогенерируемых сайтмапов,
он обязательно должен быть указан в robots.txt
5 - тул позволяет читать карту включающую в себя составную карту из нескольких файлов sitemap
6 - при возникновении ошибок, с ними можно ознакомиться в автоматически создаваемой директории Logs
