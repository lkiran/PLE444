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

export function LoginPage() {
    const {classes} = useStyles();
    return (
        <div className={classes.wrapper}>
            <Paper className={classes.form} radius={0} p={30}>
                <Title order={2} className={classes.title} ta="center" mt="md" mb={50}>
                    Welcome to Ple
                </Title>

                <TextInput label="Email address" placeholder="hello@gmail.com" size="md"/>
                <PasswordInput label="Password" placeholder="Your password" mt="md" size="md"/>
                <Checkbox label="Keep me logged in" mt="xl" size="md"/>
                <Button fullWidth mt="xl" size="md">
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