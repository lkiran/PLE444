import {HomePageNavigation} from "@/HomeModule/View/HomePageNavigation";
import {Container} from "@mantine/core";
import {UserCardItem} from "@/HomeModule/View/UserCardItem";

export function HomePage() {

    return (
        <div>
            <HomePageNavigation />

            <Container >
                <UserCardItem postedAt={"12"} body={"Lorem ipsum"} name={"semo"} image={"imageee"}/>
            </Container>
        </div>
    );
}