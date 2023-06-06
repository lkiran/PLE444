import {
    Anchor,
    Button,
    Checkbox,
    createStyles,
    Paper,
    PasswordInput,
    rem,
    Text,
    TextInput,
    Title,
} from '@mantine/core';

import BounBg from '../../../resources/BogaziciUni.jpg';
import {useForm} from "@mantine/form";
import {UserController} from "@/UserModule/Controller/UserController";
import {UserLoginDto} from "@/UserModule/Model/UserLoginDto";
import {useUserStore} from "@/UserModule/Store/UserStore";

interface params {
    userController: UserController,
}

const useStyles = createStyles((theme) => ({
    wrapper: {
        minHeight: rem(900  ),
        backgroundSize: 'cover',
        backgroundImage: `url(${BounBg.src})`,
    },

    form: {
        borderRight: `${rem(1)} solid ${
            theme.colorScheme === 'dark' ? theme.colors.dark[7] : theme.colors.gray[3]
        }`,
        minHeight: rem(900),
        maxWidth: rem(450),
        paddingTop: rem(80),

        [theme.fn.smallerThan('sm')]: {
            maxWidth: '100%',
        },
    },

    title: {
        color: theme.colorScheme === 'dark' ? theme.white : theme.black,
        fontFamily: `Greycliff CF, ${theme.fontFamily}`,
    },
}));

function GetLoginDto(values: { password: string; email: string }): UserLoginDto {
    return  { email: values.email, password: values.password};
}

export function LoginPage(props: params) {
    const {classes} = useStyles();

    const userController: UserController = props.userController;

    const setUser = useUserStore((state) => state.setUser);


    const form = useForm({
        initialValues: {
            email: '',
            password: ''
        }
    })

    async function OnButtonClick() {
        const userLoginDto: UserLoginDto = GetLoginDto(form.values);
        const result =  await userController.TryLogin(userLoginDto);

        setUser(result)
    }

    return (
        <div className={classes.wrapper}>
            <Paper className={classes.form} radius={0} p={30}>
                <Title order={2} className={classes.title} ta="center" mt="md" mb={50}>
                    Welcome to Ple
                </Title>

                <TextInput label="Email address" placeholder="hello@gmail.com" size="md" {...form.getInputProps('email')}/>
                <PasswordInput label="Password" placeholder="Your password" mt="md" size="md" {...form.getInputProps('password')}/>
                <Button type="submit" onClick={OnButtonClick} fullWidth mt="xl" size="md">
                    Login
                </Button>

                <Text ta="center" mt="md">
                    Don&apos;t have an account?{' '}
                    <Anchor<'a'> href="#" weight={700} onClick={(event) => event.preventDefault()}>
                        Register
                    </Anchor>
                </Text>
            </Paper>
        </div>
    );
}