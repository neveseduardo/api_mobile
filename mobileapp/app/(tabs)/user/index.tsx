import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import Button from '@/components/ui/Button';
import { useRouter } from 'expo-router';
import UserProfileHeader from '@/components/user/UserProfileHeader';
import { useAuth } from '@/contexts/AuthContext';

const UserScreen = () => {
	const router = useRouter();
	const { userData } = useAuth();

	return (
		<ThemedView className="flex-1 w-full p-5">
			<ThemedView className="flex items-center justify-center w-full gap-4">
				<UserProfileHeader username={userData?.name} email={userData?.email} />

				<Button onPress={() => router.push('/(tabs)/user/address')} className="w-full">
					<ThemedText>Meus endereços</ThemedText>
				</Button>
			</ThemedView>
		</ThemedView>
	);
};

export default UserScreen;