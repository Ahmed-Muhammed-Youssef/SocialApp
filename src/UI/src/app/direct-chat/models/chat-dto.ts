import { MessageDto } from "./message-dto";

export interface ChatDto{
    userId: number;
    userFirstName: string;
    userLastName: string;
    lastMessage: MessageDto;
}