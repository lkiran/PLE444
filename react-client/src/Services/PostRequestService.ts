import axios, {AxiosResponse} from "axios";

export class PostRequestService {
    async Post<T>(address: string, params: any): Promise<AxiosResponse<T>> {

        return await axios.post(address, params);
    }
}
