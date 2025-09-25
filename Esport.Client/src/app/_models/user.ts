import { AuthRole } from '@app/_services/client';

export class User {
  id!: number;
  firstName!: string;
  lastName!: string;
  username!: string;
  roles: AuthRole[] = [];
  token!: string;
}
