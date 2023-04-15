# Just Short It (damnit)!

The most KISS single user URL shortener there is.


## To simply run Just Short It in a container run:
```
docker run -e JSI_BaseUrl=<your-url> \
           -e JSI_Account__Username=<your-username> \
           -e JSI_Account__Password=<your-password> \
           <todo>
```


## In Docker Compose:
```docker-compose
version: '3.4'

services:
  just-short-it:
    container_name: JustShortIt
    image: <todo>
    environment:
      - "JSI_BaseUrl=<your-url>"
      - "JSI_Account__Username=<your-username>"
      - "JSI_Account__Password=<your-password>"
```

## Redis

By default Just Short It saves all the redirects in a in-memory distributed Cache, which get's lost 
whenever the container restarts, so if you want to keep your redirects you wanna use a redis DB.

You can confiure the connection to redis using the environment variables `JSI_Redis__ConnectionString` 
and optional `JSI_Redis__InstanceName` (it's default is "JustShortIt").

If you want to run both with compose, the most simple setup looks like this:

```docker-compose
version: '3.4'

services:
  just-short-it:
    container_name: JustShortIt
    image: <todo>
    environment:
      - "JSI_BaseUrl=<your-url>"
      - "JSI_Account__Username=<your-username>"
      - "JSI_Account__Password=<your-password>"
      - "JSI_Redis__ConnectionString=redis,password=<your-redis-password>"
  redis:
    container_name: Redis
    image: redis:alpine
    environment:
      - "REDIS_PASSWORD=<your-redis-password>"
    volumes:
      - redis:/data

volumes:
  redis:
```

There you go, now your urls survive a restart!

# License and Attribution

Just Short It by [Mia Winter](https://miawinter.de/) is licensed under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).  
Just Short It uses [Bulma](https://bulma.io/) for styling, Bulma is licensed under the [MIT License](https://github.com/jgthms/bulma/blob/master/LICENSE).


Copyright (c) 2023 Mia Winter