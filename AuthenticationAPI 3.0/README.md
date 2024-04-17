
# Authentication API
## Describtion
That API was created to safely manage user accounts in another project. It can perform operations such as creating account, logging in/out, saving confirmation code (can be sent via e-mail by another API) that is required for example to create new account or set Your new password when You forgot previous one. Every piece of data that should be available only to its owner is saved in PostgreSQL Database as encrypted bytea, so raw data from there is useless for potential hacker.


## Installation

To work properly API requires:
- PostgreSQl Database
- permission to create and read files in home directory of user that runs API
    
## Features

- Possible Encryption and Decryption of data sent to API - unique for every account (every account has own encryption keys). For example You can store in Your database data as encrypted bytea and use this API to decrypt it (only if You are logged in, because this function requires session token). There are two options of data transferring: 
            
        1. string -> encrypted byte[]
        2. List of { string ID, string Value } -> List of { string ID, byte[] Value }

        Both methods are available in reverse versions.

- Functions for updating:
        
        1. Password
        2. Username
        3. Email (requires code confirmation)
- Function for setting new password when You forgot previous one (requires code confirmation)


## Contact
If You have any questions send e-mail to

- milosz.tech@gmail.com

