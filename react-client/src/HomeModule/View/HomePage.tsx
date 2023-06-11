import {HomePageNavigation} from "@/HomeModule/View/HomePageNavigation";
import {Container} from "@mantine/core";
import {UserCardItem} from "@/HomeModule/View/UserCardItem";
import axios from "axios";
import {UriConfig} from "@/Config/UriConfig";
import {useEffect, useState} from "react";

export function HomePage() {
        const [post, setPosts] = useState(null);

        useEffect(() => {
            axios.get(UriConfig.PostsUrl).then(response => {
                setPosts(response.data);
            });
        }, []);


    return (
        <div>
            <HomePageNavigation name={"Semo"} image={"image"} tabs={["bir", "iki", "üçç"]} />

            <Container >
                {post != null ? post.map((post) => (<UserCardItem body={post.content} name={post.title} image={"image"}/>)) : (<div></div>)}

            </Container>
        </div>
    );
}