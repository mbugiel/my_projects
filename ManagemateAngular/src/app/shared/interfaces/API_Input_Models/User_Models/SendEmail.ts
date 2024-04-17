export interface SendEmail {
    username: string;
    email: string;
    password: string;
    twoStepLogin: boolean;
    template: number;
}