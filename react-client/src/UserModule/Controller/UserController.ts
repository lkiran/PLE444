import {User} from "@/UserModule/Model/UserDto";
import {UserLoginDto} from "@/UserModule/Model/UserLoginDto";
import {PostRequestService} from "@/Services/PostRequestService";
import {UriConfig} from "@/Config/UriConfig";

export class UserController {
    private _postRequestService: PostRequestService;

    constructor(postRequestService: PostRequestService) {
        this._postRequestService = postRequestService;
    }

    static GetLocalUser = () => {

        if (typeof window == 'undefined')
        {
            return {}
        }

        const getLocalSavedUser = window.localStorage.getItem("ple-app-user");
        if (getLocalSavedUser)
        {
            const user = JSON.parse(getLocalSavedUser);
            console.log(user);
            return user;
        } else
        {
            return {};
        }
    };

     TryLogin = async (userLoginDto: UserLoginDto) : Promise<UserLoginDto> => {
        let response = await this._postRequestService.Post<UserLoginDto>(UriConfig.UserLoginUrl, userLoginDto);
        return response.data ;
    };


}