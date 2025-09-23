export * from './events.service';
import { EventsService } from './events.service';
export * from './users.service';
import { UsersService } from './users.service';
export const APIS = [EventsService, UsersService];
