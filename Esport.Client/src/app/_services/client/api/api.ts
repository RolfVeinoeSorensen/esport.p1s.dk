export * from './events.service';
import { EventsService } from './events.service';
export * from './games.service';
import { GamesService } from './games.service';
export * from './news.service';
import { NewsService } from './news.service';
export * from './users.service';
import { UsersService } from './users.service';
export const APIS = [EventsService, GamesService, NewsService, UsersService];
