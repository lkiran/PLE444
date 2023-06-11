import {
    createStyles,
    Text,
    Avatar,
    Group,
    TypographyStylesProvider,
    Paper,
    rem,
} from '@mantine/core';

const useStyles = createStyles((theme) => ({
    comment: {
        padding: `${theme.spacing.lg} ${theme.spacing.xl}`,
    },

    body: {
        paddingLeft: rem(54),
        paddingTop: theme.spacing.sm,
        fontSize: theme.fontSizes.sm,
    },

    content: {
        '& > p:last-child': {
            marginBottom: 0,
        },
    },
}));

interface CommentHtmlProps {
    postedAt: string;
    body: string;
    name: string;
    image: string;
}

export function UserCardItem({ postedAt, body, name, image }: CommentHtmlProps) {
    const { classes } = useStyles();
    return (
        <Paper withBorder radius="md" className={classes.comment}>
            <Group>
                <Avatar src={image} alt={name} radius="xl" />
                <div>
                    <Text fz="sm">{name}</Text>
                    <Text fz="xs" c="dimmed">
                        {postedAt}
                    </Text>
                </div>
            </Group>
            <TypographyStylesProvider className={classes.body}>
                <div className={classes.content} dangerouslySetInnerHTML={{ __html: body }} />
            </TypographyStylesProvider>
        </Paper>
    );
}