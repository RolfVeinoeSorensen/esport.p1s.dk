import { UserRole } from './userrole';

export class User {
  id!: number;
  firstName!: string;
  lastName!: string;
  username!: string;
  role!: UserRole;
  token!: string;
}
