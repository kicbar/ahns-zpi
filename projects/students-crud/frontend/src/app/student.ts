export interface Student {
  id: number;
  firstName: string;
  lastName: string;
  age: number;
  major: string;
}

export type StudentRequest = Omit<Student, 'id'>;
