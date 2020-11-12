export interface ToDoItem {
    id?: string;
    description: string;
    createdTime?: Date;
    done: boolean;
    favorite: boolean;
    children: Array<ToDoItem>;
}
