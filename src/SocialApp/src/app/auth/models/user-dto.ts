export interface UserDTO {
  id: number;
  firstName: string;
  lastName: string;
  gender: Gender;
  age: number;
  created: string;
  lastActive: string;
  bio: string;
  relationStatus: RelationStatus;
  pictures: PictureDTO[];
}

export enum Gender {
  NotSpecified = 0,
  Male = 1,
  Female = 2
}

export enum RelationStatus {
  None = 0,
  Friend = 1,
  FriendRequested = 2,
  FriendRequestReceived = 3
}

export interface PictureDTO {
  id: number;
  url: string;
}