
export interface ClientDetails {
  id: string
  name: string
  createDate: string
  queueName: string
  isOnline: boolean
  messages: MessageClient[]
}

interface MessageClient {
  body: string
  createDate: string
  sendMessageDate: string
  sendMessageDateFormat: string
}
