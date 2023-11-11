export interface Message {
    id: number;
    content: string;
    readDate?: Date;
    sentDate: Date;
    senderPhotoUrl: string;
    senderId: number;
    recipientId: number;
}
