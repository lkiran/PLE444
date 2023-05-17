import Head from 'next/head'
import Image from 'next/image'
import { Inter } from 'next/font/google'
import styles from '@/styles/Home.module.css'
import {HeaderSimpleProps, NavBar} from "@/navigation/navbar";
import {HeaderProps} from "@mantine/core";
import {LoginPage} from "@/AuthenticationModule/View/LoginPage";
import {HomePage} from "@/HomeModule/View/HomePage";
import {UserController} from "@/UserModule/Controller/UserController";
import {PostRequestService} from "@/Services/PostRequestService";

const inter = Inter({ subsets: ['latin'] })

const navigation: HeaderSimpleProps = {links: [{link :"/home", label: "Ple"}, {link :"/home", label: "Ple"}, {link :"/home", label: "Ple"}]};

export default function Home() {
    const postRequestService: PostRequestService = new PostRequestService();
    const userController: UserController = new UserController(postRequestService);

    return (
        <>
            <Head>
                <title>PLE</title>
                <meta name="description" content="Generated by create next app" />
                <meta name="viewport" content="width=device-width, initial-scale=1" />
                <link rel="icon" href="/favicon.ico" />
            </Head>
            <main>
                <LoginPage userController={userController} />
                {/* <NavBar links={navigation.links}/> */}
                {/* <HomePage/> */}
            </main>
        </>
    )
}