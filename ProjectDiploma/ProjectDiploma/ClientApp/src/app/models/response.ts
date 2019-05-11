export enum MessageType {
  INFO = 'INFO',
  WARNING = 'WARNING',
  ERROR = 'ERROR'
}

export interface Message {
  text: string;
  messageType: MessageType;
}

export class Response<T> {
  itemResult: T;
  hasErrors: boolean;
  messages: Message[];
}
