import { Topic } from "./topic"

export interface ClientDetails {
  id: string
  name: string
  createDate: string
  queueName: string
  messages: MessageClient[]
  topics: Topic[]
}

interface MessageClient {
  body: string
  createDate: string
  sendMessageDate: string
  sendMessageDateFormat: string
}
