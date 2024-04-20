export interface UserRegister {
    username: string;
    password: string;
    email: string;
    twoStepLogin: boolean;
    emailToken: string;
}