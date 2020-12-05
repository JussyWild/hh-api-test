# Описание объекта тестирования

В работе рассматривается тестирование api hh.ru,
предоставляющего пользователям информацию о различных 
вакансиях, резюме, соискателях и тд.

Чтобы провести тесты локально, необходимо:

1. В корневой папке с Dockerfile'ом выполнить команду _docker build -t api-test ._ .
2. В корневой папке с Dockerfile'ом выполнить команду _docker run api-test_ .

## Отчет по тестированию
1. **Объект тестирования**

    Роут https://api.hh.ru/vacancies с различным параметром text.
    
2. **Место тестирования**

    Тестирование проводится автоматически в системе CI/CD.

3. **Время тестирования**

    Набор тестов с различными вариантами запросов должны проводиться автоматически при выпуске новой версии сервиса на этапе разработки.
 
4. **Краткое описание**

   В ходе тестирования было проведено 21 тест-кейсов, которые проверяют роут https://api.hh.ru/vacancies с  различным значением параметра text.
   Написаны авто-тесты. 
 
5. **Чек-лист**

    | Номер проверки | Название проверки  | Статус  | Результат |
    | :------------: | :------------: |:---------------:| :-----:|
    | 1 | запрос без агента и токена      | проведен        |   положительно |
    | 2 | запрос с hh-агентом без токена      | проведен        |   отрицательно |
    | 3 | запрос с агентом без токена | проведен        |   отрицательно |
    | 4 | запрос с hh-агентом и токеном | проведен        |   положительно |
    | 5 | запрос с text=Программист | проведен        |   положительно |
    | 6 | запрос с text=Developer | проведен        |   положительно |
    | 7 | запрос с text=Продажа оборудования | проведен        |   положительно |
    | 8 | запрос с text="Продажа оборудования" | проведен        |   положительно |
    | 9 | запрос с text=!Продажи | проведен        |   отрицательно |
    | 10 | запрос с text=Гео* | проведен        |   положительно |
    | 11 | запрос с text=пиарщик | проведен        |   положительно |
    | 12 | запрос с text=c# or c++ | проведен        |   положительно |
    | 13 | запрос с text="разработчик java" or "разработчик c#" | проведен        |   положительно |
    | 14 | запрос с text="python" and "django" | проведен        |   отрицательно |
    | 15 | запрос с text=NAME:(python or java) and COMPANY_NAME:Headhunter | проведен        |   положительно |
    | 16 | запрос с text=xss | проведен        |   положительно |
    | 17 | запрос с text=sql_injection | проведен        |   отрицательно |
    | 18 | запрос с text=repeat_1_symbol | проведен        |   положительно |
    | 19 | запрос с text=wrong_encoding | проведен        |   положительно |
    | 20 | запрос с text=only_spaces | проведен        |   положительно |
    | 21 | запрос с text=empty_line | проведен        |   положительно |
    
6. **Обнаруженные дефекты**

    | Номер проверки  | Описание дефекта |
    | :------------: | :------------: |
    | 2  | Согласно документации результатом запроса без токена должен быть BadRequest  |
    | 3  | Согласно документации результатом запроса без токена должен быть BadRequest  |
    | 9  | Согласно документации результатом запроса должны быть вакансии, каждая из которых содержит слово _продажи_  |
    | 14  | Согласно документации результатом запроса должны быть вакансии, каждая из которых содержит слова _python_ и  _django_ |
    | 17  | Согласно документации результатом запроса должен быть пустой список вакансий  |