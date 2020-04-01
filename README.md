# web_server_files
University project  
Workpath for logfile and other files - [server/server/bin/Debug/](https://github.com/polinch/web_server_files/tree/master/server/server/bin/Debug)

## Модель веб-сервера
Клиент посылает запрос Get. – Сервер в ответ передаёт файл с заданным
именем, и клиент отображает его на экране. В файле есть ссылки на другие файлы (тоже со ссылками). Клиент может выбрать ссылку и запросить
новый файл – сервер его передаёт клиенту для отображения.

## Особенности реализации
* Сервер ведет подсчет соединений
* В файле log.txt отражаются такие действия, как:
  * Запуск сервера
  * Установка соединения
  * Количество принятых и отправленных байт
  * Завершение соединения с клиентом
  * Количество соединений на момент остановки сервера
  * Остановка сервера
* После остановки сервер можно запустить снова
* Для предотвращения блокировки UI сервера используется Task
* Обрабатываются следующие ошибки:
  * Клиент пытается установить соединение с незапущенным сервером
  * Клиент пытается отправить пустое имя файла
  * Клиент отправил имя несуществующего файла
* Рабочая директория для логфайла сервера и файлов, которые на нем хранятся - [server/server/bin/Debug/](https://github.com/polinch/web_server_files/tree/master/server/server/bin/Debug)
