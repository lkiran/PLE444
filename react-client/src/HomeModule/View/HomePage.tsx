import {HomePageNavigation} from "@/HomeModule/View/HomePageNavigation";
import {Container, Grid} from "@mantine/core";
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
            <HomePageNavigation name={"Ple User"} image={"image"} tabs={["Discussion"]} />

            <Container>
                <Grid>
                    {
                        post != null
                            ? post.map((post) => (<Grid.Col xs={8} xl={12} >
                                                <UserCardItem body={post.content} name={post.title} image={"image"}/>
                                    </Grid.Col>))
                            : (<div></div>)
                    }
                </Grid>
            </Container>
        </div>
    );
}