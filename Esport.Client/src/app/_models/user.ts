import { Role } from '@app/_services/client';

export class User {
  id!: number;
  firstName!: string;
  lastName!: string;
  username!: string;
  roles: Role[] = [];
  token!: string;
}
